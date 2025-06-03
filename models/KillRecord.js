const mongoose = require('mongoose');

const KillRecordSchema = new mongoose.Schema({
  user: { type: mongoose.Schema.Types.ObjectId, ref: 'User', required: true },
  kills: { type: Number, default: 0 }
});

module.exports = mongoose.model('KillRecord', KillRecordSchema);