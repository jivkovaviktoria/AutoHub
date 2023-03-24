import styles from './NavigationBar.module.css';
import {useState} from "react";

export const NavigationBar = () => {
    const [isActive, setIsActive] = useState(false);

    const ClickHandler = () => {
        setIsActive(!isActive);
    }

    return (
        <header>
            <div className={styles.logo}>AUTOhub</div>
            <div className={`${styles.hamburger}`} onClick={ClickHandler}>
                <div className={styles.line}></div>
                <div className={styles.line}></div>
                <div className={styles.line}></div>
            </div>
            <nav className={`${styles.navBar} ${isActive ? `${styles.active}` : ''}`}>
                <ul>
                    <li><a href='/'>Home</a></li>
                    <li><a href='/Cars'>Cars</a></li>
                    <li><a href='/Add'>Add</a></li>
                    <li><a href='/Account'>Account</a></li>
                </ul>
            </nav>
        </header>
    );
}