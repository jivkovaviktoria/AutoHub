    import { useEffect, useState} from "react";
    import * as carsService from "../../services/CarService";
    import styles from "./Cars.module.css";
    import 'react-loading/dist/react-loading';
    import { Card, Filter } from '../../components';
    import Loading from "react-loading";
    import {Car} from "../car/Car";

    export const Cars = () => {
        const [cars, setCars] = useState([]);
        const [selectedCar, setSelectedCar] = useState(null);
        const [property, setProperty] = useState('brand');
        const [direction, setDirection] = useState('asc');
        const [isLoading, setIsLoading] = useState(true);

        useEffect(() => {
            const token = sessionStorage.getItem('token');
            async function fetchData() {
                const result = await carsService.GetAll();
                setCars(result.$values);
                setIsLoading(false);
            }
            fetchData();
        }, []);

        const setCarsHandler = (filteredCars) => {
            setCars(filteredCars.$values);
        }
        const propertyHandler = (e) => {
            setProperty(e.target.value);
        };

        const directionHandler = (e) => {
            setDirection(e.target.value);
        };

        const orderHandler = () => {
            const order = {Property: property, IsAscending: direction === 'asc'};
            carsService
                .OrderCars(order)
                .then((orderedCars) => { setCars((prevCars) => {return orderedCars.$values})});
        };

        const selectCarHandler = (carId) => {
            if(carId === null) setSelectedCar(null);
            else {
                carsService.GetSingle(carId)
                    .then(car => {setSelectedCar(car);
                    });
            }
        }

        const saveCarHandler = (carId) => {
            carsService.SaveCar(carId).then(r => console.log(r));
        }

        if(selectedCar){
            return <Car car={selectedCar} onSelect={() => selectCarHandler(null)}/>
        }

        return (
            <>
            {sessionStorage.getItem('token') ? (
            <div className={styles.wrapper}>
                <div className={styles["order-form"]}>
                    <label>
                        OrderBy
                        <select className={styles.select} value={property} onChange={propertyHandler}>
                            <option value="">Select a property</option>
                            <option value="brand">Make</option>
                            <option value="model">Model</option>
                            <option value="year">Year</option>
                            <option value="price">Price</option>
                        </select>
                    </label>
                    <label>
                        Direction:
                        <select className={styles.select} value={direction} onChange={directionHandler}>
                            <option value="asc">Select a direction</option>
                            <option value="asc">Ascending</option>
                            <option value="desc">Descending</option>
                        </select>
                    </label>
                    <button onClick={orderHandler}>Order</button>
                </div>
                <Filter onFilter={setCarsHandler} filterName="Price"/>
                <Filter onFilter={setCarsHandler} filterName="Year"/>
                <div className={styles["cars-wrapper"]}>
                    {isLoading ? (
                        <div className={styles.loading}>
                            <Loading type="spin" color="#0072ff" />
                        </div>
                    ) : (
                        cars.length > 0 &&
                        cars.map((car) => (
                            <Card
                                key={car.id}
                                car={car}
                                onInfoClick={selectCarHandler}
                                onSaveClick={saveCarHandler}
                            />
                        ))
                    )}
                </div>
            </div>
                    ) : (
                    <div>You are not authorized to access this page. Please login or register.</div>
                )}
                </>
        );
    }
