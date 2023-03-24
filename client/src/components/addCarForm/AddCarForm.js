import { useRef } from 'react';
import styles from './AddCarForm.module.css';
import * as CarsService from "../../services/CarsService";

import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

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

        CarsService.Add(carData);

        modelInputRef.current.value = '';
        brandInputRef.current.value = '';
        yearInputRef.current.value = '';
        priceInputRef.current.value = '';
        descriptionInputRef.current.value = '';
        imageInputRef.current.value = '';
    }

    const notify = () => toast.success("Added successfully!");

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
                <button type="submit" onClick={notify}>Add</button>
                <ToastContainer theme='dark' autoClose={3000} limit={3} typ/>
            </form>
        </div>
    );
}