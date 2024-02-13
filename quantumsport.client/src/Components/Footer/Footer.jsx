import React from 'react';
import styles from "./Footer.module.scss";
import { NavLink } from 'react-router-dom';
import { ReactComponent as Logo } from "../../img/Logo.svg";

function Footer() {


    return (
        <section className={styles.footer}>
            <div className={styles.footerContacts}>
                <h3 className={styles.footerContactsTitle}>Контакти</h3>
                <p className={styles.footerContactsName}>Спортивний комплекс “EnergyGym”</p>
                <p className={styles.footerContactsAddress}>м. Київ,  Дніпровська набережна, 14</p>
                <a className={styles.footerContactsPhone} href="tel:+380630808080">+380 (63) 080-80-80</a>
                <a className={styles.footerContactsPhone} href="tel:+380670808080">+380 (67) 080-80-80</a>
                <a className={styles.footerContactsPhone} href="tel:+380500808080">+380 (50) 080-80-80</a>
            </div>
            <NavLink to="/" className={styles.logo}>
                <Logo className={styles.logoImg} />
                <h2 className={styles.logoTitle}>ENERGYGYM</h2>
            </NavLink>
            <div className={styles.footerSchedule}>
                <h3 className={styles.footerScheduleTitle}>Графік роботи</h3>
                <p className={styles.footerScheduleText}>Пн-пт......................7:00 - 22:00</p>
                <p className={styles.footerScheduleText}>сб...........................9:00 - 19:00</p>
                <p className={styles.footerScheduleText}>нд...........................9:00 - 15:00</p>
            </div>
        </section>
    );
}

export default Footer;