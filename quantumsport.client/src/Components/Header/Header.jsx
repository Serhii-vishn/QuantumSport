import React from 'react';
import styles from "./Header.module.scss";
import { ReactComponent as Logo } from "../../img/Logo.svg";
import { NavLink } from 'react-router-dom';

function Header() {
    return (
        <div className={styles.header}>
            <NavLink to="/" className={styles.headerLogo}>
                <Logo className={styles.logoImg} />
                <h2 className={styles.logoTitle}>ENERGYGYM</h2>
            </NavLink>
            <nav className={styles.headerNav}>
                <ul className={styles.headerNavList}>
                    <li className={styles.headerNavListItem}>
                        <NavLink to="/" className={styles.headerNavListItemLink}>
                            головна
                        </NavLink></li>
                    <li className={styles.headerNavListItem}>
                        <NavLink to="/services" className={styles.headerNavListItemLink}>
                            послуги
                        </NavLink>
                        <ul  className={styles.headerNavListItemInfo}>
                            <li><NavLink className={styles.infoLink}>Записатись на тренування</NavLink></li>
                            <li><NavLink className={styles.infoLink}>Замовити індивідуальну програму тренувань</NavLink></li>
                            <li><NavLink className={styles.infoLink}>Замовити індивідуальний план харчування</NavLink></li>
                        </ul>
                    </li>
                    <li className={styles.headerNavListItem}>
                        <NavLink to="/team" className={styles.headerNavListItemLink}>
                            команда
                        </NavLink>
                        {/* <ul className={styles.headerNavListItemInfo}>

                        </ul> */}
                        </li>
                    <li className={styles.headerNavListItem}>
                        <NavLink to="/sections" className={styles.headerNavListItemLink}>
                            спортивні секції
                        </NavLink>
                        {/* <div className={styles.headerNavListItemInfo}>

                        </div> */}
                        </li>
                    <li className={styles.headerNavListItem}>
                        <NavLink to="/blog" className={styles.headerNavListItemLink}>
                            блог
                        </NavLink></li>
                    <li className={styles.headerNavListItem}>
                        <NavLink to="/aboutUs" className={styles.headerNavListItemLink}>
                            про energygym
                        </NavLink></li>
                </ul>
                <button className={styles.headerNavBtn}>МОЇ ТРЕНУВАННЯ</button>
            </nav>
        </div>
    );
}

export default Header;