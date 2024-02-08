import React from 'react';
import { Route, Routes } from 'react-router-dom';
import MainPage from "./pages/MainPage/MainPage";

function AppRoutes() {
  return (
    <Routes>
        <Route path="/" element={<MainPage/>}/>
        <Route path="/services" element={<MainPage/>}/>
        <Route path="/team" element={<MainPage/>}/>
        <Route path="/sections" element={<MainPage/>}/>
        <Route path="/blog" element={<MainPage/>}/>
        <Route path="/aboutUs" element={<MainPage/>}/>
    </Routes>
  );
}

export default AppRoutes;
