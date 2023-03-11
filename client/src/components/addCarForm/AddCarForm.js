import styles from './AddCarForm.module.css';
import * as CarsService from "../../services/CarsService";

export const AddCarForm = ({onCarAdd}) => {
    const carAddHandler = (e) => {
        e.preventDefault();

        const data = new FormData(e.target);
        const carData = Object.fromEntries(data);

        CarsService.Add(carData)
            .then(car => console.log(car));
    }

    return (
        <div className={styles.wrapper}>
            <form onSubmit={carAddHandler}>
                <label>
                    Model:
                    <input type="text" name="model"/>
                </label>
                <label>
                    Brand:
                    <input type="text" name="brand"/>
                </label>
                <label>
                    Year:
                    <input type="number" name="year"/>
                </label>
                <label>
                    Price:
                    <input type="number" name="price"/>
                </label>
                <label>
                    Description:
                    <input type="text" name="description"/>
                </label>
                <label>
                    Image:
                    <input type="text" name="imageUrl"/>
                </label>
                <button type="submit">Add</button>
            </form>
        </div>
    );
}