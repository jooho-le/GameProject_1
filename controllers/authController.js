// controllers/authController.js
const User   = require('../models/User');
const bcrypt = require('bcryptjs');
const jwt    = require('jsonwebtoken');

exports.register = async (req, res) => {
  const { username, email, password } = req.body;
  try {
    // 이메일 중복 검사
    if (await User.findOne({ email })) {
      return res.status(400).json({ msg: '이미 가입된 이메일입니다.' });
    }
    // 비밀번호 해시
    const salt   = await bcrypt.genSalt(10);
    const hashed = await bcrypt.hash(password, salt);
    // 유저 생성
    const user = new User({ username, email, password: hashed });
    await user.save();
    // JWT 발급
    const token = jwt.sign({ id: user._id }, process.env.JWT_SECRET, {
      expiresIn: process.env.JWT_EXPIRES_IN
    });
    // 응답: 토큰 + 유저 정보(JSON)
    res.json({
      token,
      user: {
        id: user._id,
        username: user.username,
        email: user.email,
        stageStatus: user.stageStatus
      }
    });
  } catch (err) {
    console.error('Register error:', err);
    res.status(500).send('서버 오류');
  }
};

exports.login = async (req, res) => {
  const { email, password } = req.body;
  try {
    // 유저 조회
    const user = await User.findOne({ email });
    if (!user) return res.status(400).json({ msg: '가입 정보가 없습니다.' });
    // 비밀번호 검증
    const match = await bcrypt.compare(password, user.password);
    if (!match) return res.status(400).json({ msg: '비밀번호가 틀렸습니다.' });
    // JWT 발급
    const token = jwt.sign({ id: user._id }, process.env.JWT_SECRET, {
      expiresIn: process.env.JWT_EXPIRES_IN
    });
    // 응답
    res.json({
      token,
      user: {
        id: user._id,
        username: user.username,
        email: user.email,
        stageStatus: user.stageStatus
      }
    });
  } catch (err) {
    console.error('Login error:', err);
    res.status(500).send('서버 오류');
  }
};
