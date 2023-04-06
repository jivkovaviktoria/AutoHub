import {useRef, useState} from 'react';
import styles from './AddCarForm.module.css';
import * as CarsService from "../../services/CarService";

import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import * as ImagesService from "../../services/ImageService";

export const AddCarForm = ({onCarAdd}) => {
    const modelInputRef = useRef(null);
    const brandInputRef = useRef(null);
    const yearInputRef = useRef(null);
    const priceInputRef = useRef(null);
    const descriptionInputRef = useRef(null);

    const [file, setFile] = useState(null);

    const handleFileSelection = (e) => {
        e.preventDefault();
        
        const selectedFile = e.target.files;
        setFile(selectedFile);
    };

    const carAddHandler = async (e) => {
        e.preventDefault();

        const data = new FormData(e.target);

        const fileData = await ImagesService.UploadManyImages(file);
        console.log(fileData.data.$values);

        const carData = {
            model: modelInputRef.current.value,
            brand: brandInputRef.current.value,
            year: yearInputRef.current.value,
            price: priceInputRef.current.value,
            description: descriptionInputRef.current.value,
            imageUrl: fileData.data.$values[0],
            images: fileData.data.$values
        };

        await CarsService.Add(carData);

        modelInputRef.current.value = '';
        brandInputRef.current.value = '';
        yearInputRef.current.value = '';
        priceInputRef.current.value = '';
        descriptionInputRef.current.value = '';
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
                    Images:
                    <input type="file" name="images" multiple onChange={handleFileSelection}/>
                </label>
                <button type="submit" onClick={notify}>Add</button>
                <ToastContainer theme='dark' autoClose={3000} limit={3} typ/>
            </form>
        </div>
    );
}