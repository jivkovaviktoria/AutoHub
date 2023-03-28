import { useEffect, useState } from "react";
import * as CarsService from "../services/CarsService";
import { Card } from "../components/card/Card";
import styles from "./Account.module.css";

export const Account = () => {
    const [cars, setCars] = useState([]);
    const [savedCars, setSavedCars] = useState([]);
    const [activeTab, setActiveTab] = useState("my-cars");
    const [user, setUser] = useState(null);

    useEffect(() => {
        const token = sessionStorage.getItem("token");
        async function fetchData() {
            const result = await CarsService.GetCarsByUser();
            const saved = await CarsService.GetSavedCars();
            setCars(result.$values[0].cars.$values);
            setSavedCars(saved.$values[0].cars.$values);
            //const user = await CarsService.GetUser();
            //setUser(user);
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
                        {cars.map((car) => (
                            <Card key={car.id} car={car} />
                        ))}
                    </div>
                );
            case "saved-cars":
                return (
                    <div className={styles["cars-wrapper"]}>
                        {savedCars.map((car) => (
                            <Card key={car.id} car={car} />
                        ))}
                    </div>
                );
            case "profile":
                return (
                    <div>
                        <h2>Profile</h2>

                            <div>
                                <p>Name: ViktoriyaV</p>
                                <p>Email: jivkovaviktoria@gmail.com</p>
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
};
