import React from 'react';
import { NavLink } from 'react-router-dom';
import photo from "../../img/mainPhoto1.png";
import { ReactComponent as DumbbellWeight } from "../../img/dumbbellWeight.svg";
import { ReactComponent as Dumbbell } from "../../img/dumbbell.svg";
import { ReactComponent as BoxingGloves } from "../../img/boxingGloves.svg";
import crossfit from "../../img/crossfit.png";
import fitness from "../../img/fitness.png";
import boxing from "../../img/boxing.png";
import styles from "./MainPage.module.scss";


function MainPage() {
    return (
        <>
            <div className={styles.photoSection}>
                <img className={styles.photoSectionImg} src={photo} alt="Фото головної сторінки" />
                <div className={styles.photoSectionInfo}>
                    <h1 className={styles.photoSectionInfoTitle}>Тренуй своє тіло та розум</h1>
                    <p className={styles.photoSectionInfoText}>Приєднуйтесь до нас і відкривайте
                        новий рівень фізичної підготовки.</p>
                    <NavLink to="/services" className={styles.photoSectionInfolink}>розпочни сьогодні!</NavLink>
                </div>
            </div>
            <section className={styles.sectionsWrapper}>
                <ul className={styles.sectionList}>
                    <li className={styles.sectionListItem}>
                        <div className={styles.sectionItemInfoWrapper}>
                            <div className={styles.sectionItemInfoTextWrapper}>
                                <h3 className={styles.sectionItemInfoTitle}>БОКС</h3>
                                <BoxingGloves className={styles.sectionItemInfoSvg} />
                                <p className={styles.sectionItemInfoText}>Контактний вид спорту, єдиноборство, в якому спортсмени наносять один одному удари кулаками в спеціальних рукавичках.Перемога боксера у випадку, якщо суперник збитий з ніг і не може піднятися протягом десяти секунд (нокаут) або після отриманої травми.</p>
                            </div>
                            <NavLink to="/section/boxing" className={styles.sectionItemInfoLink}>Записатися</NavLink>
                        </div>
                        <img className={styles.sectionItemImg} src={boxing} alt="Бокс фото" />
                    </li>
                    <li className={styles.sectionListItem}>
                        <img className={styles.sectionItemImg} src={crossfit} alt="Crossfit фото" />
                        <div className={styles.sectionItemInfoWrapper}>
                            <div className={styles.sectionItemInfoTextWrapper}>
                                <h3 className={styles.sectionItemInfoTitle}>CROSSFIT</h3>
                                <DumbbellWeight className={styles.sectionItemInfoSvg} />
                                <p className={styles.sectionItemInfoText}>Кросфіт — молодий спортивний напрямок, що стрімко розвивається та набирає популярність у Києві та всіх великих містах України.CrossFit — це новий погляд на спорт, здоровий спосіб життя і самого себе, свої можливості та прагнення.</p>
                            </div>
                            <NavLink to="/section/crossfit" className={styles.sectionItemInfoLink}>Записатися</NavLink>
                        </div>
                    </li>
                    <li className={styles.sectionListItem}>
                        <div className={styles.sectionItemInfoWrapper}>
                            <div className={styles.sectionItemInfoTextWrapper}>
                                <h3 className={styles.sectionItemInfoTitle}>ФІТНЕС</h3>
                                <Dumbbell className={styles.sectionItemInfoSvg} />
                                <p className={styles.sectionItemInfoText}>Рух — це життя. Фітнес — це рух для сучасних людей, які хочуть жити насичено та активно. Cпорт може змінити на краще життя кожної людини, головне — знайти напрямок, який буде вам до душі та людей, які підтримають ваше прагнення до здоров’я та краси.</p>
                            </div>
                            <NavLink to="/section/fitness" className={styles.sectionItemInfoLink}>Записатися</NavLink>
                        </div>
                        <img className={styles.sectionItemImg} src={fitness} alt="Фітнес фото" />
                    </li>
                </ul>
            </section>
        </>

    );
}

export default MainPage;