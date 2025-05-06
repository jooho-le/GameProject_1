// middleware/authMiddleware.js
const jwt  = require('jsonwebtoken');
const User = require('../models/User');

exports.protect = async (req, res, next) => {
  let token;
  if (
    req.headers.authorization &&
    req.headers.authorization.startsWith('Bearer ')
  ) {
    token = req.headers.authorization.split(' ')[1];
  }
  if (!token) {
    return res.status(401).json({ msg: '로그인 토큰이 없습니다.' });
  }
  try {
    const decoded = jwt.verify(token, process.env.JWT_SECRET);
    req.user = await User.findById(decoded.id).select('-password');
    if (!req.user) return res.status(404).json({ msg: '유저를 찾을 수 없습니다.' });
    next();
  } catch (err) {
    console.error('Auth middleware error:', err);
    res.status(401).json({ msg: '유효하지 않은 토큰입니다.' });
  }
};
