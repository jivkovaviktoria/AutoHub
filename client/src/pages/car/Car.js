import styles from './Car.module.css';

export const Car = ({car, onSelect}) => {
    const selectHandle = () => {
        onSelect();
    }

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
                <img src={car.imageUrl}/>
            </div>
        </div>
    );
}
