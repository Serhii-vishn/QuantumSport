import React, { useState } from 'react';
import styles from "./Header.module.scss";
import { ReactComponent as Logo } from "../../img/Logo.svg";
import { NavLink } from 'react-router-dom';
import classNames from 'classnames';

function Header() {
    const [isServices, setIsServices] = useState(false);
    const [isTeam, setIsTeam] = useState(false);
    const [isSections, setIsSections] = useState(false);
    const [isActiveMenu, setIsActiveMenu] = useState(false);

    return (
        <div className={styles.header}>
            <NavLink to="/" className={styles.headerLogo}>
                <Logo className={styles.logoImg} />
                <h2 className={styles.logoTitle}>ENERGYGYM</h2>
            </NavLink>
            <nav className= {classNames(styles.headerNav,isActiveMenu? styles.active:null)}>
                <ul className={styles.headerNavList}>
                    <li className={styles.headerNavListItem}>
                        <NavLink to="/" className={styles.headerNavListItemLink}>
                            головна
                        </NavLink></li>
                    <li className={styles.headerNavListItem} onMouseEnter={() => setIsServices(true)} onMouseLeave={() => setIsServices(false)}>
                        <p className={styles.headerNavListItemLink}>
                            послуги
                        </p>
                        {isServices ?
                            <ul className={classNames(styles.headerNavListItemInfo, styles.headerNavListItemInfoServices)}>
                                <li><NavLink to="/schedule" className={styles.infoLink}>Записатись на тренування</NavLink></li>
                                <li><button type='button'  className={styles.infoLink}>Замовити індивідуальну програму тренувань</button></li>
                                <li><button type='button'  className={styles.infoLink}>Замовити індивідуальний план харчування</button></li>
                            </ul>
                            : ""}
                    </li>
                    <li className={styles.headerNavListItem} onMouseEnter={() => setIsTeam(true)} onMouseLeave={() => setIsTeam(false)}>
                        <p className={styles.headerNavListItemLink}>
                            команда
                        </p>
                        {isTeam ?
                            <ul className={classNames(styles.headerNavListItemInfo, styles.headerNavListItemInfoTeam)}>
                                <li><NavLink to="/staff/coaches" className={styles.infoLink}>Тренери</NavLink></li>
                                <li><NavLink to="/staff/nutritionists" className={styles.infoLink}>Дієтологи</NavLink></li>
                            </ul>
                            : ""}
                    </li>
                    <li className={styles.headerNavListItem} onMouseEnter={() => setIsSections(true)} onMouseLeave={() => setIsSections(false)}>
                        <p className={styles.headerNavListItemLink}>
                            спортивні секції
                        </p>
                        {isSections ?
                            <ul className={classNames(styles.headerNavListItemInfo, styles.headerNavListItemInfoSections)}>
                                <li><NavLink to="/section/boxing" className={styles.infoLink}>Бокс</NavLink></li>
                                <li><NavLink to="/section/crossfit" className={styles.infoLink}>Crossfit</NavLink></li>
                                <li><NavLink to="/section/fitness" className={styles.infoLink}>Фітнес</NavLink></li>
                            </ul>
                            : ""}
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
                <button type='button' className={styles.headerNavBtn}>МОЇ ТРЕНУВАННЯ</button>
            </nav>
            <div className={classNames(styles.menu, isActiveMenu ? styles.active : null)} onClick={() => { setIsActiveMenu((val) => !val) }}>
                <span className={styles.burger}></span>
            </div>
        </div>
    );
}

export default Header;