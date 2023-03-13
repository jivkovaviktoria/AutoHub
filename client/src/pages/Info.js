import styles from './Info.module.css';

export const Info = ({car, onClose}) => {
    return (
        <div className={styles.overlay} onClick={onClose}>
            <div className={styles['info-window']}>
                <h1>{car.id}</h1>
            </div>
        </div>
    );
}