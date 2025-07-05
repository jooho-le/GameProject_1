const express = require('express');
const router = express.Router();
const killController = require('../controllers/killController');

router.post('/kill', killController.recordKill);
router.get('/leaderboard/kill', killController.getLeaderboard);

module.exports = router;
