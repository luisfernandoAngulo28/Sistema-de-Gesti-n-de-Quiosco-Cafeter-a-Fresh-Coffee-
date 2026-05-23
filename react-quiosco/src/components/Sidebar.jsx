import { Link } from "react-router-dom"
import { LogOut, Sun, Moon } from "lucide-react"
import useQuisco from "../hooks/useQuiosco"
import Categoria from "./Categoria"
import { useAuth } from "../hooks/useAuth"
import { useDarkMode } from "../hooks/useDarkMode"

export default function Sidebar() {
    const { categorias, pedido } = useQuisco()
    const { logout, user } = useAuth({ middleware: 'auth' })
    const [dark, toggleDark] = useDarkMode()

    const colores = ['bg-indigo-600', 'bg-violet-600', 'bg-blue-600', 'bg-teal-600']
    const colorIdx = user?.name ? user.name.charCodeAt(0) % colores.length : 0
    const colorClase = colores[colorIdx]

    const handleLogout = () => {
        if (pedido.length > 0) {
            const confirmar = window.confirm(
                `Tenés ${pedido.length} producto${pedido.length > 1 ? 's' : ''} en el carrito. ¿Querés cerrar sesión de todas formas?`
            )
            if (!confirmar) return
        }
        logout()
    }

    return (
        <aside className="md:w-64 h-screen flex flex-col bg-white dark:bg-gray-900 border-r border-gray-100 dark:border-gray-700/50 flex-shrink-0 transition-colors duration-200">
            <div className="px-5 pt-5 pb-4 border-b border-gray-100 dark:border-gray-700/50 flex items-center justify-between">
                <img className="w-28" src="img/logo.svg" alt="Logo" />
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
                    {user?.name ? (
                        <div className={`w-9 h-9 rounded-full ${colorClase} flex items-center justify-center text-white font-bold text-base flex-shrink-0 select-none shadow-sm`}>
                            {user.name.charAt(0).toUpperCase()}
                        </div>
                    ) : (
                        <div className="w-9 h-9 rounded-full bg-gray-100 dark:bg-gray-700 animate-pulse flex-shrink-0" />
                    )}
                    <div className="min-w-0 flex-1">
                        {user?.name ? (
                            <>
                                <p className="text-sm font-semibold text-gray-800 dark:text-gray-100 truncate leading-tight">{user.name}</p>
                                <Link to="/mis-pedidos" className="text-xs text-indigo-500 hover:text-indigo-700 dark:hover:text-indigo-400 font-medium transition-colors">
                                    Mis pedidos anteriores →
                                </Link>
                            </>
                        ) : (
                            <>
                                <div className="h-3 w-24 bg-gray-100 dark:bg-gray-700 rounded animate-pulse mb-1.5" />
                                <div className="h-2.5 w-32 bg-gray-100 dark:bg-gray-700 rounded animate-pulse" />
                            </>
                        )}
                    </div>
                </div>
            </div>

            <nav className="flex-1 overflow-y-auto px-3 py-4 space-y-0.5">
                <p className="text-xs font-bold text-gray-400 dark:text-gray-500 uppercase tracking-widest px-3 mb-3">Menú</p>
                {categorias.length === 0
                    ? [...Array(6)].map((_, i) => (
                        <div key={i} className="flex items-center gap-3 px-4 py-3 rounded-xl animate-pulse">
                            <div className="w-8 h-8 bg-gray-100 dark:bg-gray-700 rounded-full flex-shrink-0" />
                            <div className="h-3 bg-gray-100 dark:bg-gray-700 rounded w-24" />
                        </div>
                    ))
                    : categorias.map(categoria => (
                        <Categoria key={categoria.id} categoria={categoria} />
                    ))
                }
            </nav>

            <div className="p-4 border-t border-gray-100 dark:border-gray-700/50">
                <button
                    type="button"
                    className="w-full py-2.5 px-4 rounded-xl bg-red-50 dark:bg-red-900/20 text-red-500 dark:text-red-400 font-semibold text-sm hover:bg-red-100 dark:hover:bg-red-900/40 transition-colors duration-150 flex items-center justify-center gap-2"
                    onClick={handleLogout}
                >
                    <LogOut className="w-4 h-4" />
                    Cerrar Sesión
                </button>
            </div>
        </aside>
    )
}
