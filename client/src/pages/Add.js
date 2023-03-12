import {AddCarForm} from "../components/addCarForm/AddCarForm";
import styles from './Add.module.css';

export const Add = () => {
    return (
        <div className={styles.wrapper}>
            <AddCarForm/>
        </div>
    );
}