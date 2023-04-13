import {useState} from "react";
import * as CarsService from '../../services/CarService';
import styles from "../yearFilter/YearFilter.module.css";

export const YearFilter = ({onFilter, onClose}) => {
    const [minYear, setMinYear] = useState(-1);
    const [maxYear, setMaxYear] = useState(-1);

    const handleFilter = () => {
        CarsService.FilterByYear({Min: minYear, Max: maxYear})
            .then(result => onFilter(result));
        onClose();
    }

    return (
        <div className={styles.overlay}>
            <div className={styles['filter-window']}>
                <div className={styles['filter-details']}>
                    <button className={styles.close} onClick={onClose}>X</button>
                    <label>Min:</label>
                    <input type='number' onInput={e => setMinYear(e.target.value)}/>
                    <label>Max:</label>
                    <input type='number' onInput={e => setMaxYear(e.target.value)}/>
                    <button onClick={handleFilter}>Filter</button>
                </div>
            </div>
        </div>
    );
}