const KillRecord = require('../models/KillRecord');

exports.recordKill = async (req, res) => {
  const { userId, kills } = req.body;

  try {
    let record = await KillRecord.findOne({ userId });

    if (record) {
      record.kills += kills;
      await record.save();
    } else {
      record = new KillRecord({ userId, kills });
      await record.save();
    }

    res.status(200).send("Kill count updated");
  } catch (err) {
    res.status(500).send("Error recording kill");
  }
};

exports.getLeaderboard = async (req, res) => {
  try {
    const topPlayers = await KillRecord.find().sort({ kills: -1 }).limit(10);
    res.json(topPlayers);
  } catch (err) {
    res.status(500).send("Error fetching leaderboard");
  }
};
