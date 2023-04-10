import image from '../../images/homepage.jpg'
import styles from './Home.module.css'

export const Home = () => {
    return (
        <div>
            <img className={styles.image} src={image}/>
            <h1 className={styles.inscription}>FIND THE RIGHT CAR FOR YOU.</h1>
        </div>
    );
}