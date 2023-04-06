import styles from './Info.module.css';

export const Info = ({car, onClose}) => {
    return (
        <div className={styles.overlay} onClick={onClose}>
            <div className={styles['info-window']}>
                <div className={styles.content}>
                    <div className={styles['car-details']}>
                        <p>
                            <strong className={styles.heading}> {car.model} {car.brand} </strong>
                        </p>
                        <p>Year: <strong>{car.year}</strong></p>
                        <p>Price: <strong>{car.price}</strong></p>
                        <p>
                            <strong>{car.description}</strong>
                        </p>
                        <img src={car.imageUrl}/>
                    </div>
                </div>
            </div>
        </div>
    );
}