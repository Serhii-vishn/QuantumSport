import React from 'react';
import { NavLink } from 'react-router-dom';
import photo from "../../img/mainPhoto1.png";
import styles from "./MainPage.module.scss";


function MainPage() {
    return (
        <div className={styles.photoSection}>
            <img className={styles.photoSectionImg} src={photo} alt="Фото головної сторінки" />
            <div className={styles.photoSectionInfo}>
                <h1 className={styles.photoSectionInfoTitle}>Тренуй своє тіло та розум</h1>
                <p className={styles.photoSectionInfoText}>Приєднуйтесь до нас і відкривайте
                    новий рівень фізичної підготовки.</p>
                <NavLink className={styles.photoSectionInfolink}>розпочни сьогодні!</NavLink>
            </div>

        </div>
    );
}

export default MainPage;