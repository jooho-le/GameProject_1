// server/controllers/authController.js

const bcrypt = require('bcrypt');
const jwt = require('jsonwebtoken');
const User = require('../models/User');

// 회원가입
exports.signup = async (req, res) => {
  const { username, password } = req.body;
  // 1) 입력값 검사
  if (!username || !password) {
    return res.status(400).json({ error: 'ID와 비밀번호를 모두 입력해주세요.' });
  }

  try {
    // 2) 비밀번호 해시 생성
    const hash = await bcrypt.hash(password, 10);

    // 3) 사용자 저장
    const user = new User({ username, password: hash });
    await user.save();

    return res.status(201).json({ message: '회원가입 성공' });
  } catch (err) {
    console.error('Signup error:', err);
    // 중복 키 에러 처리 (이미 존재하는 사용자)
    if (err.code === 11000) {
      return res.status(409).json({ error: '이미 존재하는 사용자입니다.' });
    }
    return res.status(500).json({ error: '서버 오류' });
  }
};

// 로그인
exports.login = async (req, res) => {
  const { username, password } = req.body;
  if (!username || !password) {
    return res.status(400).json({ error: 'ID와 비밀번호를 모두 입력해주세요.' });
  }

  try {
    const user = await User.findOne({ username });
    if (!user) {
      return res.status(401).json({ error: '사용자를 찾을 수 없습니다.' });
    }

    const match = await bcrypt.compare(password, user.password);
    if (!match) {
      return res.status(401).json({ error: '비밀번호가 올바르지 않습니다.' });
    }

    const token = jwt.sign({ id: user._id }, process.env.JWT_SECRET, { expiresIn: '1h' });
    return res.json({ token });
  } catch (err) {
    console.error('Login error:', err);
    return res.status(500).json({ error: '서버 오류' });
  }
};
