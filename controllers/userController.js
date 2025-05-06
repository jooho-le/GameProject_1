// controllers/userController.js
// protect 미들웨어로 걸러진 후에 호출됨
exports.getMe = async (req, res) => {
  const u = req.user; // authMiddleware에서 세팅
  res.json({
    user: {
      id: u._id,
      username: u.username,
      email: u.email,
      stageStatus: u.stageStatus
    }
  });
};
