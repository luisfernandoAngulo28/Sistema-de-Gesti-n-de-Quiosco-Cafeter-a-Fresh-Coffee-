import { Outlet } from 'react-router-dom'
import { ToastContainer } from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css'
import AdminSidebar from "../components/AdminSidebar"
import { useAuth } from '../hooks/useAuth'

export default function AdminLayout() {
    useAuth({ middleware: 'admin' })

    return (
        <div className='md:flex bg-slate-50 dark:bg-gray-950 min-h-screen transition-colors duration-200'>
            <AdminSidebar />
            <main className='flex-1 h-screen overflow-y-scroll p-6 bg-slate-50 dark:bg-gray-950 transition-colors duration-200'>
                <Outlet />
            </main>
            <ToastContainer theme="colored" position="top-right" />
        </div>
    )
}
