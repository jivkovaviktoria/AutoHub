import {Card} from "../../components";

import styles from './User.module.css';
import * as carsService from "../../services/CarService";
import {useState} from "react";
import {Car} from "../car/Car";

export const User = ({user}) => {
    const [selectedCar, setSelectedCar] = useState(null);

    const selectCarHandler = (carId) => {
        if(carId === null) setSelectedCar(null);
        else {
            carsService.GetSingle(carId)
                .then(car => {setSelectedCar(car);
                });
        }
    };

    const saveCarHandler = (carId) => { carsService.SaveCar(carId).then(r => console.log(r)); };

    if(selectedCar){
        return <Car car={selectedCar} onSelect={() => selectCarHandler(null)}/>
    }

    return(
        <div>
            <h1 className={styles.username}>{user.userName} - 0888888888</h1> {/* Add user phone number here */}
            <div className={styles['cars-wrapper']}>
            {user.cars.$values.map((car) => (
                <Card
                    key={car.id}
                    car={car}
                    onInfoClick={() => selectCarHandler(car.id)}
                    onSaveClick={() => saveCarHandler(car.id)}
                />
            ))}
            </div>
        </div>
    );
}