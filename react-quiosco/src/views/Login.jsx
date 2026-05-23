import { createRef, useState } from 'react'
import { Link } from 'react-router-dom'
import Alerta from '../components/Alerta'
import { useAuth } from '../hooks/useAuth'

export default function Login() {
    const emailRef = createRef()
    const passwordRef = createRef()
    const [errores, setErrores] = useState([])
    const [cargando, setCargando] = useState(false)

    const { login } = useAuth({ middleware: 'guest', url: '/' })

    const handleSubmit = async e => {
        e.preventDefault()
        setCargando(true)
        await login({ email: emailRef.current.value, password: passwordRef.current.value }, setErrores)
        setCargando(false)
    }

    return (
        <div className='w-full max-w-sm'>
            <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-100">Bienvenido</h1>
            <p className="text-gray-400 dark:text-gray-500 text-sm mt-1">Inicia sesión para realizar tu pedido</p>

            <form onSubmit={handleSubmit} noValidate className="mt-8 space-y-4">
                {errores.map((error, i) => <Alerta key={i}>{error}</Alerta>)}

                <div>
                    <label className="block text-sm font-semibold text-gray-600 dark:text-gray-300 mb-1.5" htmlFor="email">
                        Correo electrónico
                    </label>
                    <input
                        type="email"
                        id="email"
                        ref={emailRef}
                        placeholder="tu@email.com"
                        className="w-full px-4 py-2.5 rounded-xl border border-gray-200 dark:border-gray-600 bg-gray-50 dark:bg-gray-800 text-gray-800 dark:text-gray-100 placeholder-gray-400 dark:placeholder-gray-500 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:border-transparent transition"
                    />
                </div>

                <div>
                    <label className="block text-sm font-semibold text-gray-600 dark:text-gray-300 mb-1.5" htmlFor="password">
                        Contraseña
                    </label>
                    <input
                        type="password"
                        id="password"
                        ref={passwordRef}
                        placeholder="Mínimo 8 caracteres"
                        className="w-full px-4 py-2.5 rounded-xl border border-gray-200 dark:border-gray-600 bg-gray-50 dark:bg-gray-800 text-gray-800 dark:text-gray-100 placeholder-gray-400 dark:placeholder-gray-500 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:border-transparent transition"
                    />
                </div>

                <button
                    type="submit"
                    disabled={cargando}
                    className="w-full bg-indigo-600 hover:bg-indigo-700 text-white py-3 rounded-xl font-bold text-sm uppercase tracking-wide transition-colors duration-150 disabled:opacity-60 mt-2"
                >
                    {cargando ? 'Iniciando...' : 'Iniciar Sesión'}
                </button>
            </form>

            <p className="text-center text-sm text-gray-400 dark:text-gray-500 mt-6">
                ¿No tienes cuenta?{' '}
                <Link to="/auth/registro" className="text-indigo-600 font-semibold hover:text-indigo-800 dark:hover:text-indigo-400">
                    Crear cuenta
                </Link>
            </p>
        </div>
    )
}
