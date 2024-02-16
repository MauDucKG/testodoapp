import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './Routes/App.jsx'
import './index.css'
import {RouterProvider,createBrowserRouter} from 'react-router-dom'
import Login from './Routes/Login.jsx'
import ViewTask, { loader as ViewTaskLoader } from './components/ViewTask.jsx';
const router =
createBrowserRouter(
  [
    {
        path: '/',
        element: <App />,
        children:[
            { path: '/:taskID', element: <ViewTask />, loader: ViewTaskLoader}
        ]
        },
        {path:'/login',element: <Login />},
  ]
);

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router}/>
  </React.StrictMode>,
)
