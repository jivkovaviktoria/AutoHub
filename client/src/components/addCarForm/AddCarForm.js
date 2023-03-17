import { useRef } from 'react';
import styles from './AddCarForm.module.css';
import * as CarsService from "../../services/CarsService";

export const AddCarForm = ({onCarAdd}) => {
    const modelInputRef = useRef(null);
    const brandInputRef = useRef(null);
    const yearInputRef = useRef(null);
    const priceInputRef = useRef(null);
    const descriptionInputRef = useRef(null);
    const imageInputRef = useRef(null);

    const carAddHandler = (e) => {
        e.preventDefault();

        const data = new FormData(e.target);
        const carData = Object.fromEntries(data);

        CarsService.Add(carData, localStorage.getItem('token'))
            .then(car => console.log(car));

        modelInputRef.current.value = '';
        brandInputRef.current.value = '';
        yearInputRef.current.value = '';
        priceInputRef.current.value = '';
        descriptionInputRef.current.value = '';
        imageInputRef.current.value = '';
    }

    return (
        <div className={styles.wrapper}>
            <div className={styles.heading}>AUTOhub | Add car</div>
            <form onSubmit={carAddHandler}>
                <label>
                    Model:
                    <input type="text" name="model" ref={modelInputRef}/>
                </label>
                <label>
                    Brand:
                    <input type="text" name="brand" ref={brandInputRef}/>
                </label>
                <label>
                    Year:
                    <input type="number" name="year" ref={yearInputRef}/>
                </label>
                <label>
                    Price:
                    <input type="number" name="price" ref={priceInputRef}/>
                </label>
                <label>
                    Description:
                    <input type="text" name="description" ref={descriptionInputRef}/>
                </label>
                <label>
                    Image:
                    <input type="text" name="imageUrl" ref={imageInputRef}/>
                </label>
                <button type="submit">Add</button>
            </form>
        </div>
    );
}