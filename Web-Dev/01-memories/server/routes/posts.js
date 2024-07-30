import express from 'express';

import { getPosts, createPost } from '../controllers/posts.js';

const router = express.Router();


// http://localhost:5000/posts (only because we have app.use('/posts', postRoutes); in 01-memories/server/index.js)
router.get('/', getPosts);
router.post('/', createPost);

export default router;