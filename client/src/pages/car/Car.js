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
        console.log(images);
    }, []);


    return (
        <div className={styles.container}>
            <button className={styles['back-button']} onClick={selectHandle}>Back</button>
            <div className={styles['heading-container']}>
                    <h1 className={styles.heading}>{car.model} {car.brand}</h1>
                    <h1 className={styles.price}>{car.price}$</h1>
            </ div>
            <div className={styles.textContainer}>
                <p className={styles.text}>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries</p>
            </div>
            <div className={styles['images-container']}>
                <div>{images.length > 0 && images.map((image) => (<img key={image.id} src={image.url}/>))}</div>
            </div>
        </div>
    );
}
