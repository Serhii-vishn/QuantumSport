import React from 'react';
import { Route, Routes } from 'react-router-dom';
import MainPage from "./pages/MainPage/MainPage";

function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<MainPage />} />
      <Route path="/blog" />
      <Route path="/aboutUs" />

      <Route path="/section/*" >
        <Route path='boxing' />
        <Route path='crossfit' />
        <Route path='fitness' />
      </Route>

      <Route path="/staff/*">
        <Route path='coaches' />
        <Route path='nutritionists' />
        <Route path="coach/:id" />
        <Route path="nutritionist/:id" />
      </Route>

      <Route path="/schedule"/>
    </Routes>
  );
}

export default AppRoutes;
