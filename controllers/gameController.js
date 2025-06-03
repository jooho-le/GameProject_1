const StageRecord = require('../models/StageRecord');

exports.recordClear = async (req, res) => {
  const { userId, stage } = req.body;
  await new StageRecord({ user: userId, stage }).save();
  res.json({ message: '기록 저장됨' });
};

exports.getLeaderboard = async (req, res) => {
  const top = await StageRecord.aggregate([
    { $group: { _id: '$user', maxStage: { $max: '$stage' } } },
    { $sort: { maxStage: -1 } },
    { $limit: 10 },
    { $lookup: { from: 'users', localField: '_id', foreignField: '_id', as: 'user' } },
    { $unwind: '$user' },
    { $project: { username: '$user.username', maxStage: 1 } }
  ]);
  res.json(top);
};
