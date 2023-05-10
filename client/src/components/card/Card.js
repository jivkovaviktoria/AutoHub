import styles from './Card.module.css';
import {BsBookmarkPlus, BsBookmarkPlusFill} from 'react-icons/bs';
import {useEffect, useState} from "react";

export const Card = (props) => {
    const [isSaved, setISaved] = useState(false);

    const saveHandler = () => {
        if(!isSaved) props.onSaveClick(props.car.id);
        setISaved(!isSaved);
    }

    return (
        <div className={styles.card}>
            <div className={styles['card-body']}>
                <img className={styles.image} src={props.car.imageUrl}/>
                <h2 className={styles['card-title']}>{props.car.model}</h2>
                <p className={styles['card-description']}>{props.car.brand}</p>
            </div>
            <button className={styles['card-button']} onClick={() => props.onInfoClick(props.car.id)}>Full Info</button>
            {isSaved ?
                <BsBookmarkPlusFill className={styles.favourite} onClick={saveHandler}/> :
                <BsBookmarkPlus className={styles.favourite} onClick={saveHandler} />
            }
        </div>
    );
}