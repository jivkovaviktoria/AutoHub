import styles from './Car.module.css';
import * as ImageService from '../../services/ImageService';
import {useEffect, useState} from "react";
import * as userService from "../../services/UserService";
import {User} from "../user/User";

export const Car = ({car, onSelect}) => {
    const [images, setImages] = useState([]);
    const [user, setUser] = useState({});
    const [isUserSelected, setIsUserSelected] = useState(false);

    const selectHandle = () => { onSelect();}

    useEffect(()  => {
        const handleImages = async () => {
            const result = await ImageService.GetByCarId(car.id)
                .then(images => setImages(images.$values));
        }
        handleImages();

        const handleUser = async () => {
            await userService.GetUserInfo(car.userId).then(result => setUser(result.$values[0]));
        }
        handleUser();
    }, []);

    const selectUserHandler = () => { setIsUserSelected(true); }

    if(isUserSelected) { return (<User user={user}/>); }

    return (
        <div className={styles.container}>
            <button className={styles['back-button']} onClick={selectHandle}>Back</button>
            <div className={styles['heading-container']}>
                    <h1 className={styles.heading}>{car.model} {car.brand}</h1>
                    <h1 className={styles.price}>{car.price}$</h1>
            </ div>
            <div className={styles.textContainer}>
                <p className={styles.text}>{car.description}</p>
            </div>
            <div className={styles['images-container']}>
                <div>{images.length > 0 && images.map((image) => (<img key={image.id} src={image.url}/>))}</div>
            </div>
            <div className={styles['user-container']}>
                <button className={styles['user-button']} onClick={selectUserHandler}>{user.userName}</button>
            </div>
        </div>
    );
}
