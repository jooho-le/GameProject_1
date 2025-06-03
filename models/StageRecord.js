const mongoose = require('mongoose');
const StageRecordSchema = new mongoose.Schema({
  user: { type: mongoose.Schema.Types.ObjectId, ref: 'User', required: true },
  stage: { type: Number, required: true },
  clearedAt: { type: Date, default: Date.now }
});
module.exports = mongoose.model('StageRecord', StageRecordSchema);