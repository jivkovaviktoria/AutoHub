import {useState} from "react";
import * as CarsService from '../../services/CarService';
import styles from "../priceFilter/PriceFilter.module.css";

export const PriceFilter = ({onFilter, onClose}) => {
    const [minPrice, setMinPrice] = useState(-1);
    const [maxPrice, setMaxPrice] = useState(-1);

    const handleFilter = () => {
        CarsService.FilterByPrice({Min: minPrice, Max: maxPrice})
            .then(result => onFilter(result));
        onClose();
    }

    return (
            <div className={styles.overlay}>
                <div className={styles['filter-window']}>
                    <div className={styles['filter-details']}>
                        <button className={styles.close} onClick={onClose}>X</button>
                        <label>Min:</label>
                        <input type='number' onInput={e => setMinPrice(e.target.value)}/>
                        <label>Max:</label>
                        <input type='number' onInput={e => setMaxPrice(e.target.value)}/>
                        <button onClick={handleFilter}>Filter</button>
                    </div>
                </div>
            </div>
    );
}