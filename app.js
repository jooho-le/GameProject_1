// app.js
require('dotenv').config();
const express   = require('express');
const connectDB = require('./config/db');

const app = express();
connectDB();

// JSON 바디 파싱
app.use(express.json());

// 라우트 등록
app.use('/api/auth', require('./routes/authRoutes'));
app.use('/api/users', require('./routes/userRoutes'));

// 기본 라우트
app.get('/', (req, res) => res.send('Unity 서버 준비 완료!'));

const PORT = process.env.PORT || 4000;
app.listen(PORT, () =>
  console.log(`서버 실행 중… http://localhost:${PORT}`)
);
