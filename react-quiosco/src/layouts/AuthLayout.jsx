import { Outlet } from 'react-router-dom'

export default function AuthLayout() {
    return (
        <div className='min-h-screen bg-slate-50 dark:bg-gray-950 flex items-center justify-center p-4 transition-colors duration-200'>
            <div className='w-full max-w-4xl flex flex-col md:flex-row items-center gap-12 md:gap-20'>
                <div className='flex flex-col items-center md:items-start flex-shrink-0'>
                    <img
                        src='/img/logo.svg'
                        alt='Logo Fresh Coffee'
                        className='w-48 md:w-64'
                    />
                    <p className='mt-4 text-gray-400 dark:text-gray-500 text-sm text-center md:text-left'>
                        El mejor café de la ciudad,<br />directo a tu mesa.
                    </p>
                </div>

                <div className='bg-white dark:bg-gray-900 rounded-2xl border border-gray-100 dark:border-gray-700/50 shadow-sm p-8 w-full max-w-sm'>
                    <Outlet />
                </div>
            </div>
        </div>
    )
}
