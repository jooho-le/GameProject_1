// models/User.js
const mongoose = require('mongoose');

const userSchema = new mongoose.Schema({
  username:    { type: String, required: true, unique: true },
  email:       { type: String, required: true, unique: true },
  password:    { type: String, required: true },
  stageStatus: { type: Number, default: 0 },   // 클리어한 스테이지 인덱스
  createdAt:   { type: Date, default: Date.now }
});

module.exports = mongoose.model('User', userSchema);
