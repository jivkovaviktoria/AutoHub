import styles from './Card.module.css';

export const Card = (props) => {
    return (
        <div className={styles.card}>
            <div className={styles['card-body']}>
                <img className={styles.image} src={props.car.imageUrl}/>
                <h2 className={styles['card-title']}>{props.car.model}</h2>
                <p className={styles['card-description']}>{props.car.brand}</p>
            </div>
            <button className={styles['card-button']}>Full Info</button>
        </div>
    );
}