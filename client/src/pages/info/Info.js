import styles from './Info.module.css';
import * as userService from "../../services/UserService";
import {useState, useEffect} from "react";
import {User} from "../user/User";

export const Info = ({car, onClose}) => {
    const [user, setUser] = useState({});
    const [isUserSelected, setIsUserSelected] = useState(false);

    useEffect(() => {
        const handleUser = async () => {
            await userService.GetUserInfo(car.userId).then(result => setUser(result.$values[0]));
        }
        handleUser();
    }, []);

    const selectUserHandler = () => {
        setIsUserSelected(true);
    }

    if(isUserSelected){
        return (<User user={user}/>);
    }
    console.log(user);

    return (
        <div className={styles.overlay} onClick={onClose}>
            <div className={styles['info-window']}>
                <div className={styles.content}>
                    <div className={styles['car-details']}>
                        <p>
                            <strong className={styles.heading}> {car.model} {car.brand} </strong>
                        </p>
                        <p>Year: <strong>{car.year}</strong></p>
                        <p>Price: <strong>{car.price}</strong></p>
                        <p>
                            <strong>{car.description}</strong>
                        </p>
                        <img src={car.imageUrl}/>
                        <p><a onClick={selectUserHandler}>{user.userName}</a></p>
                    </div>
                </div>
            </div>
        </div>
    );
}