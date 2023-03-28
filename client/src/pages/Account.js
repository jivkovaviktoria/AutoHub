import styles from "./Account.module.css";
import { useEffect, useState } from "react";

import * as CarsService from "../services/CarsService";
import * as UserService from "../services/UserService";
import { Card } from "../components/card/Card";

export const Account = () => {
    const [cars, setCars] = useState([]);
    const [savedCars, setSavedCars] = useState([]);
    const [activeTab, setActiveTab] = useState("profile");
    const [user, setUser] = useState([]);

    useEffect(() => {
        const token = sessionStorage.getItem("token");
        async function fetchData() {
            const result = await CarsService.GetCarsByUser();
            const saved = await CarsService.GetSavedCars();

            setCars(result.$values[0].cars.$values);
            setSavedCars(saved.$values[0].cars.$values);

            const user = await UserService.GetUser();
            setUser(user);
        }
        fetchData();
    }, []);

    const handleTabChange = (tab) => {
        setActiveTab(tab);
    };

    const renderTabContent = () => {
        switch (activeTab) {
            case "my-cars":
                return (
                    <div className={styles["cars-wrapper"]}>
                        {cars.map((car) => (<Card key={car.id} car={car} />))}
                    </div>
                );
            case "saved-cars":
                return (
                    <div className={styles["cars-wrapper"]}>
                        {savedCars.map((car) => (<Card key={car.id} car={car} />))}
                    </div>
                );
            case "profile":
                return (
                    <div>
                        <h2>Profile</h2>
                            <div>
                                <p>Name: {user.userName}</p>
                                <p>Email: {user.email}</p>
                            </div>
                    </div>
                );
            default:
                return null;
        }
    };

    return (
        <div className={styles.container}>
            <div className={styles.menu}>
                <button
                    className={activeTab === "my-cars" ? styles.active : ""}
                    onClick={() => handleTabChange("my-cars")}
                >
                    My Cars
                </button>
                <button
                    className={activeTab === "saved-cars" ? styles.active : ""}
                    onClick={() => handleTabChange("saved-cars")}
                >
                    Saved Cars
                </button>
                <button
                    className={activeTab === "profile" ? styles.active : ""}
                    onClick={() => handleTabChange("profile")}
                >
                    Profile
                </button>
            </div>
            {renderTabContent()}
        </div>
    );

}

