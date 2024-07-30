import useStyles from 'posts.styles.js';
import Post from "./Post/post";
const Posts = () => {
    const classes = useStyles();
    return (
        <>
            <h1>POSTS</h1>
            <Post />
            <Post />
        </>
    );
}

export default Posts;