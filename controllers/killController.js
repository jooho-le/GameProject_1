const KillRecord = require('../models/KillRecord');

exports.recordKill = async (req, res) => {
  const { userId, kills } = req.body;
  const kr = await KillRecord.findOneAndUpdate(
    { user: userId },
    { $inc: { kills: kills } },
    { new: true, upsert: true }
  );
  res.json({ message: '킬 기록됨', kills: kr.kills });
};

exports.getKillLeaderboard = async (req, res) => {
  const top = await KillRecord.find()
    .sort({ kills: -1 })
    .limit(10)
    .populate('user', 'username');
  res.json(
    top.map(r => ({ username: r.user.username, kills: r.kills }))
  );
};