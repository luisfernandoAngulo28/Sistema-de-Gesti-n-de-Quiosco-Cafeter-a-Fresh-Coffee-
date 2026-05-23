import { Link, useLocation } from 'react-router-dom'
import { Sun, Moon, UtensilsCrossed } from 'lucide-react'
import { useAuth } from '../hooks/useAuth'
import { useDarkMode } from '../hooks/useDarkMode'

export default function AdminSidebar() {
    const { logout, user } = useAuth({ middleware: 'auth' })
    const { pathname } = useLocation()
    const [dark, toggleDark] = useDarkMode()

    const links = [
        {
            to: '/admin',
            label: 'Órdenes',
            exact: true,
            icon: (
                <svg xmlns="http://www.w3.org/2000/svg" className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.8}>
                    <path strokeLinecap="round" strokeLinejoin="round" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
                </svg>
            )
        },
        {
            to: '/admin/productos',
            label: 'Productos',
            exact: false,
            icon: (
                <svg xmlns="http://www.w3.org/2000/svg" className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.8}>
                    <path strokeLinecap="round" strokeLinejoin="round" d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10" />
                </svg>
            )
        },
        {
            to: '/admin/mesas',
            label: 'Mesas',
            exact: false,
            icon: <UtensilsCrossed className="w-5 h-5" />
        }
    ]

    return (
        <aside className="md:w-64 h-screen flex flex-col border-r border-gray-100 dark:border-gray-700/50 bg-white dark:bg-gray-900 flex-shrink-0 transition-colors duration-200">
            <div className="p-5 border-b border-gray-100 dark:border-gray-700/50 flex items-center justify-between">
                <img src="/img/logo.svg" alt="Logo" className="w-28" />
                <button
                    onClick={toggleDark}
                    className="w-8 h-8 rounded-lg flex items-center justify-center text-gray-400 hover:text-indigo-600 hover:bg-indigo-50 dark:hover:bg-indigo-900/30 dark:hover:text-indigo-400 transition-colors"
                    title={dark ? 'Modo claro' : 'Modo oscuro'}
                >
                    {dark ? <Sun className="w-4 h-4" /> : <Moon className="w-4 h-4" />}
                </button>
            </div>

            <div className="px-4 py-3 border-b border-gray-100 dark:border-gray-700/50">
                <div className="flex items-center gap-3">
                    <div className="w-9 h-9 rounded-full bg-indigo-600 flex items-center justify-center text-white font-bold text-sm flex-shrink-0 select-none">
                        {user?.name?.charAt(0).toUpperCase() ?? 'A'}
                    </div>
                    <div className="min-w-0">
                        <p className="text-sm font-semibold text-gray-800 dark:text-gray-100 truncate">{user?.name ?? 'Admin'}</p>
                        <span className="text-xs text-indigo-500 font-medium">Administrador</span>
                    </div>
                </div>
            </div>

            <nav className="flex-1 px-3 py-4 space-y-0.5">
                <p className="text-xs font-bold text-gray-400 dark:text-gray-500 uppercase tracking-widest px-3 mb-3">
                    Panel Admin
                </p>
                {links.map(link => {
                    const activo = link.exact ? pathname === link.to : pathname.startsWith(link.to)
                    return (
                        <Link
                            key={link.to}
                            to={link.to}
                            className={`flex items-center gap-3 px-3 py-2.5 rounded-xl text-sm font-semibold transition-all duration-150
                                ${activo
                                    ? 'bg-indigo-600 text-white shadow-sm'
                                    : 'text-gray-600 dark:text-gray-400 hover:bg-gray-50 dark:hover:bg-gray-800 hover:text-indigo-600 dark:hover:text-indigo-400'
                                }`}
                        >
                            <span className={activo ? 'text-white' : 'text-gray-400 dark:text-gray-500'}>{link.icon}</span>
                            {link.label}
                        </Link>
                    )
                })}
            </nav>

            <div className="p-4 border-t border-gray-100 dark:border-gray-700/50">
                <button
                    type="button"
                    onClick={logout}
                    className="w-full py-2.5 px-4 rounded-xl bg-red-50 dark:bg-red-900/20 text-red-500 dark:text-red-400 font-semibold text-sm hover:bg-red-100 dark:hover:bg-red-900/40 transition-colors"
                >
                    Cerrar Sesión
                </button>
            </div>
        </aside>
    )
}
