import styles from './Car.module.css';
import * as ImageService from '../../services/ImageService';
import {useEffect, useState} from "react";

export const Car = ({car, onSelect}) => {
    const [images, setImages] = useState([]);

    const selectHandle = () => {
        onSelect();
    }

    useEffect(()  => {
        const handleImages = async () => {
            const result = await ImageService.GetByCarId(car.id)
                .then(images => setImages(images.$values));
        }
        handleImages();
    }, []);


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
        </div>
    );
}
