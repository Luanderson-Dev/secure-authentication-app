'use client';

/* eslint-disable @typescript-eslint/no-explicit-any */
import { api } from '@/lib/api';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useState } from 'react';

export default function LoginPage() {
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');
	const [error, setError] = useState('');
	const router = useRouter();

	const handleLogin = async (e: React.FormEvent) => {
		e.preventDefault();
		setError('');

		try {
			await api.post('/auth/login', { email, password });
			router.push('/dashboard');
			router.refresh();
		} catch (err: any) {
			setError(err.response?.data?.message || 'Credenciais inválidas.');
		}
	};

	return (
		<main className='flex min-h-screen items-center justify-center bg-gray-50 p-4'>
			<div className='bg-white p-8 rounded-lg shadow-md w-full max-w-md text-black'>
				<h1 className='text-2xl font-bold mb-6 text-center'>
					Bem-vindo de volta
				</h1>
				{error && (
					<p className='text-red-500 mb-4 text-center'>{error}</p>
				)}

				<form
					onSubmit={handleLogin}
					className='flex flex-col gap-4'
				>
					<input
						type='email'
						value={email}
						onChange={(e) => setEmail(e.target.value)}
						placeholder='E-mail'
						className='border p-2 rounded'
						required
					/>
					<input
						type='password'
						value={password}
						onChange={(e) => setPassword(e.target.value)}
						placeholder='Senha'
						className='border p-2 rounded'
						required
					/>
					<button
						type='submit'
						className='bg-blue-600 text-white p-2 rounded hover:bg-blue-700 transition'
					>
						Entrar
					</button>
				</form>
				<div className='mt-4 text-center'>
					<Link
						href='/register'
						className='text-blue-600 hover:underline'
					>
						Não tem conta? Cadastre-se
					</Link>
				</div>
			</div>
		</main>
	);
}
