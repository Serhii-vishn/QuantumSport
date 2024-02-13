import React from 'react';
import { Route, Routes } from 'react-router-dom';
import MainPage from "./pages/MainPage/MainPage";

function AppRoutes() {
  return (
    <Routes>
        <Route path="/" element={<MainPage/>}/>
        <Route path="/blog"/>
        <Route path="/aboutUs"/>
        
        <Route path="/services"/>

        <Route path="/section/*" >
          <Route path='boxing'/>
          <Route path='crossfit'/>
          <Route path='fitness'/>
        </Route>

        <Route path="/personnel/*">
          <Route path='coachesList'/>
          <Route path='nutritionistsList'/>
        </Route>

        <Route path="/worker/:id/*">
          <Route path='coach'/>
          <Route path='nutritionist'/>
        </Route>
    </Routes>
  );
}

export default AppRoutes;
