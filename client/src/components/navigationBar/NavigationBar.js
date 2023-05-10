import styles from './NavigationBar.module.css';
import {useState} from "react";

export const NavigationBar = () => {
    const [isActive, setIsActive] = useState(false);

    const ClickHandler = () => {
        setIsActive(!isActive);
    }

    const logoutHandler = () => {
        sessionStorage.removeItem('token');
    }

    return (
        <header>
            <div className={styles.logo}>AUTO<span>hub</span></div>
            <div className={`${styles.hamburger}`} onClick={ClickHandler}>
                <div className={styles.line}></div>
                <div className={styles.line}></div>
                <div className={styles.line}></div>
            </div>
            <nav className={`${styles.navBar} ${isActive ? `${styles.active}` : ''}`}>
                <ul>
                    <li><a href='/'>Home</a></li>
                    <li><a href='/cars/Cars'>Cars</a></li>
                    <li><a href='/add/Add'>Add</a></li>
                    <li><a href='/account/Account'>Account</a></li>
                    <li><a href='/Auth' onClick={logoutHandler}>Logout</a></li>
                </ul>
            </nav>
        </header>
    );
}