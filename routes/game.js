const router = require('express').Router();
const { recordClear, getLeaderboard } = require('../controllers/gameController');
router.post('/record', recordClear);
router.get('/leaderboard', getLeaderboard);
module.exports = router;