require('dotenv').config();
const express = require('express');
const mongoose = require('mongoose');
const authRoutes = require('./routes/auth');
const gameRoutes = require('./routes/game');
const killRoutes = require('./routes/kill');

mongoose.connect(process.env.MONGO_URI)
  .then(() => console.log('MongoDB connected'))
  .catch(err => console.error(err));

const app = express();
app.use(express.json());
app.use('/api/auth', authRoutes);
app.use('/api/game', gameRoutes);
app.use('/api/game', killRoutes);

const port = process.env.PORT || 4000;
app.listen(port, () => console.log(`Server running on ${port}`));