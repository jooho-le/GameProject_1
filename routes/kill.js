const router = require('express').Router();
const { recordKill, getKillLeaderboard } = require('../controllers/killController');

router.post('/kill', recordKill);
router.get('/leaderboard/kill', getKillLeaderboard);

module.exports = router;